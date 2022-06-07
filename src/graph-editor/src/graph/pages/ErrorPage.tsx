import React, {FC} from "react";
import CenteredContainer from "components/CentredContainer";

const ErrorPage: FC<{ graphId: string }> = React.memo(({graphId}) => {

  return (
    <CenteredContainer>
      <h1 className="text-danger">
        Error occurred while connecting to graph <span className="text-primary"> {graphId}</span>
      </h1>
    </CenteredContainer>
  );
});
ErrorPage.displayName = "ErrorPage";
export default ErrorPage;