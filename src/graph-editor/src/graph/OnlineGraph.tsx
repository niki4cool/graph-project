import React, { FC } from "react";
import { useServerSynchronization } from "graph/hooks";
import ConnectingPage from "graph/pages/ConnectingPage";
import ErrorPage from "graph/pages/ErrorPage";
import Graph from "graph/Graph";
import { Button, Modal } from "react-bootstrap";
import { useNavigate } from "react-router-dom";
import Highlight from "components/Highlight";
import { HubConnection } from "@microsoft/signalr";

const OnlineGraph: FC<{ graphId: string }> = React.memo(({ graphId }) => {
    const navigate = useNavigate();
    const { connected, error, graphDeleted, connection } = useServerSynchronization(graphId);

    if (graphDeleted)
        return (
            <Modal show={true} centered>
                <Modal.Body className="text-center">
                    <h1>Graph <Highlight text={graphId} /> was deleted.</h1>
                    <Button variant="light" onClick={() => navigate("/")} className="w-100 mt-3">Return to main page</Button>
                </Modal.Body>
            </Modal>
        );

    if (error)
        return <ErrorPage graphId={graphId} error={error} />;

    if (!connected)
        return <ConnectingPage graphId={graphId} />;

    return <Graph graphId={graphId} connection={connection as HubConnection} />;

});
OnlineGraph.displayName = "OnlineGraph";
export default OnlineGraph;