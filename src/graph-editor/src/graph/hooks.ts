import { useEffect, useState } from "react";
import { useAppDispatch, useAppSelector } from "store";
import graphDataSlice, { Graph, GraphData, graphDataSelector, GraphNode, nodesSelector } from "graph/graphDataSlice";
import { HubConnection, HubConnectionBuilder, LogLevel } from "@microsoft/signalr";
import { BASE_URL } from "vars";
import { authUser } from "../auth/authApi";

export const useGraphDataCopy = () => {
    const data = useAppSelector(graphDataSelector);
    const [dataCopy, setDataCopy] = useState<GraphData>();

    useEffect(() => {
        setDataCopy(structuredClone(data));
        console.log("useDataCopy");
        console.log(data);
    }, [data]);

    return dataCopy;
};

export const useServerSynchronization = (graphId?: string) => {
    const dispatch = useAppDispatch();
    const [connection, setConnection] = useState<HubConnection>();
    const graphData = useAppSelector(state => state.graph.data);
    const [ready, setReady] = useState(false);
    const [error, setError] = useState();
    const token = authUser().token;
    const [graphUpdate, setDataUpdate] = useState<{ graph: Graph, isUpdating: boolean }>();
    const [graphDeleted, setGraphDeleted] = useState(false);   

    
    useEffect(() => {
        if (!ready || !graphUpdate)
            return;

        if (graphUpdate.isUpdating) {
            dispatch(graphDataSlice.actions.setGraphData(graphUpdate.graph));
        }

    }, [ready, graphUpdate?.graph]);

    useEffect(() => {
        connection?.on("OnGraphUpdate", (graph: Graph) => {
            console.log(graph);
            setDataUpdate({ graph: graph as Graph, isUpdating: true });            
        });
        connection?.on("OnGraphDelete", () => {
            setGraphDeleted(true);
        });
    }, [connection, dispatch]);

    useEffect(() => {
        if (!graphId)
            return;

        const newConnection = new HubConnectionBuilder()
            .withUrl(`${BASE_URL}/api/signalr/graph?graphId=${graphId}`,
                { accessTokenFactory: () => "" + token })
            .withAutomaticReconnect()
            .configureLogging(LogLevel.None)
            .build();

        newConnection.start()
            .then(() => {
                setConnection(newConnection);
                newConnection.send("RequestUpdate")
                    .then(() => {
                        setReady(true);
                    });
            })
            .catch(reason => {
                console.log(reason);
                setError(reason)
            });

        return () => {
            newConnection?.stop();
        };
    }, [graphId]);

    return { connected: ready, error, graphDeleted, connection };
};

export const useFindNode = () => {    
    const nodes = useAppSelector(nodesSelector);
    return (nodeId?: string | number): GraphNode | undefined => {
        if (nodeId === undefined)
            return;
        return nodes.find(x => x.id === nodeId);
    };
};