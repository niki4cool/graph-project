import React, { FC, useEffect, useState } from "react";
import { Navigate, useNavigate, useParams } from "react-router-dom";
import { graphsApi } from "graph/graphsApi";
import { isFetchBaseQueryError } from "rtkQuery";
import ConnectingPage from "graph/pages/ConnectingPage";
import NotFoundPage from "graph/pages/NotFoundPage";
import ErrorPage from "graph/pages/ErrorPage";
import OnlineGraph from "graph/OnlineGraph";

const GraphPage: FC = React.memo(() => {
    const [putGraph, mutation] = graphsApi.usePutGraphMutation();
    const { graphId } = useParams();
    const navigate = useNavigate();
    const {
        error: getError,
        isSuccess: getSuccess,
        isLoading
    } = graphsApi.useGetGraphQuery(graphId || "", {});
    const [graphCreated, setGraphCreated] = useState(false);

    var error = "";
    useEffect(() => {
        if (getSuccess && !isLoading)
            setGraphCreated(true);
    }, [getSuccess, isLoading]);

    if (!graphId)
        return <Navigate to="/" />;

    if (graphCreated)
        return <OnlineGraph graphId={graphId} />;

    if (isFetchBaseQueryError(getError) && getError.status === 401) {
        return <Navigate to="../login" />;
    }

    if (isFetchBaseQueryError(getError) && getError.status === 404) {
        return <NotFoundPage
            graphId={graphId}
            onDismiss={() => navigate("/")}
            onCreate={() => setGraphCreated(true)}
        />;
    }

    if (getError)
        return <ErrorPage graphId={graphId} error={error} />;

    return <ConnectingPage graphId={graphId} />;

});
GraphPage.displayName = "GraphPage";
export default GraphPage;