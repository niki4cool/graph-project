import {useEffect, useState} from "react";
import {useAppDispatch, useAppSelector} from "store";
import graphDataSlice, {GraphData, graphDataSelector, GraphNode} from "graph/graphDataSlice";
import {HubConnection, HubConnectionBuilder, LogLevel} from "@microsoft/signalr";
import {BASE_URL} from "vars";

export const useGraphDataCopy = () => {
  const data = useAppSelector(graphDataSelector);
  const [dataCopy, setDataCopy] = useState<GraphData>();

  useEffect(() => {
    setDataCopy(structuredClone(data));
  }, [data]);

  return dataCopy;
};

export const useServerSynchronization = (graphId?: string) => {
  const dispatch = useAppDispatch();
  const [connection, setConnection] = useState<HubConnection>();
  const graphData = useAppSelector(state => state.graphData);
  const [ready, setReady] = useState(false);
  const [error, setError] = useState();
  const [dataUpdate, setDataUpdate] = useState<{ data: GraphData, isUpdating: boolean }>();
  const [graphDeleted, setGraphDeleted] = useState(false);

  useEffect(() => {
    if (!ready || !dataUpdate)
      return;

    if (dataUpdate.isUpdating)
      dispatch(graphDataSlice.actions.setGraphData(dataUpdate.data));

  }, [ready, dataUpdate?.data]);

  useEffect(() => {
    if (!dataUpdate?.isUpdating)
      connection?.send("UpdateGraph", graphData);

    setDataUpdate({data: graphData, isUpdating: false});
  }, [graphData]);

  useEffect(() => {
    connection?.on("OnGraphUpdate", (data: GraphData) => {
      setDataUpdate({data, isUpdating: true});
    });
    connection?.on("OnGraphDelete", () => {
      setGraphDeleted(true);
    });
  }, [connection, dispatch]);

  useEffect(() => {
    if (!graphId)
      return;

    const newConnection = new HubConnectionBuilder()
      .withUrl(`${BASE_URL}/api/signalr/graph?graphId=${graphId}`)
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
      .catch(reason => setError(reason));

    return () => {
      newConnection?.stop();
    };
  }, [graphId]);

  return {connected: ready, error, graphDeleted};
};

export const useFindNode = () => {
  const nodes = useAppSelector(state => state.graphData.nodes);

  return (nodeId?: string | number): GraphNode | undefined => {
    if (nodeId === undefined)
      return;

    return nodes.find(x => x.id === nodeId);
  };
};