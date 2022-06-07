import React, {FC} from "react";
import {Spinner} from "react-bootstrap";
import CenteredContainer from "components/CentredContainer";
import Highlight from "components/Highlight";

const ConnectingPage: FC<{ graphId: string }> = React.memo(({graphId}) => {

  return (
    <CenteredContainer>
      <h1 className="mb-4">
        Connecting to graph <Highlight text={graphId}/>
      </h1>
      <Spinner animation={"border"}/>
    </CenteredContainer>
  );
});
ConnectingPage.displayName = "ConnectingPage";
export default ConnectingPage;