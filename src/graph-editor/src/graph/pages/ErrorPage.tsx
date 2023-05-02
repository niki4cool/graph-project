import React, { FC } from "react";
import CenteredContainer from "components/CentredContainer";
import LoginButton from "../../components/auth/LoginButton";

const ErrorPage: FC<{ graphId: string, error: string }> = React.memo(({ graphId, error }) => {

    return (
        <CenteredContainer>
            <h1 className="text-danger">
                <h1>{error}</h1><b />
                Error occurred while connecting to graph <span className="text-primary"> {graphId}</span>
            </h1>
        </CenteredContainer>
    );
});
ErrorPage.displayName = "ErrorPage";
export default ErrorPage;