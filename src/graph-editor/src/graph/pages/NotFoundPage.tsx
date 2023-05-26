import React, { FC, useEffect } from "react";
import CenteredContainer from "components/CentredContainer";
import { Button, Spinner } from "react-bootstrap";
import { graphsApi } from "graph/graphsApi";
import Highlight from "components/Highlight";
import { useNavigate } from "react-router-dom";

export interface NotFoundPageProps {
    graphId: string;
    onCreate?: () => void;
    onDismiss?: () => void;
}

const NotFoundPage: FC<NotFoundPageProps> = React.memo(({ graphId, onCreate, onDismiss }) => {
    const navigate = useNavigate();
    const [putGraph, { isLoading, isSuccess, isError, error }] = graphsApi.usePutGraphMutation();

    useEffect(() => {
        if (isSuccess)
            onCreate && onCreate();
        if (error) {
            navigate("../login");
        }
    }, [isSuccess, onCreate, error]);

    return (
        <CenteredContainer>
            <h1>
                Graph <Highlight text={graphId} /> not exist.
            </h1>            
            {isLoading
                ? <Spinner animation="border" />
                : (
                    <div>
                        <Button variant="secondary" className="me-5" onClick={onDismiss}>
                            Ok
                        </Button>                        
                    </div>
                )
            }
        </CenteredContainer >
    );
});
NotFoundPage.displayName = "NotFoundPage";
export default NotFoundPage;