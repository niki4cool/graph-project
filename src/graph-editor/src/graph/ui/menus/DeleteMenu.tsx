import React, {FC} from "react";
import {Button, ButtonGroup, Modal, ModalBody, ModalProps} from "react-bootstrap";
import {graphsApi} from "graph/graphsApi";
import Highlight from "components/Highlight";

export interface DeleteMenuProps extends ModalProps {
  graphId: string;
  onDelete?: () => void;
}

const DeleteMenu: FC<DeleteMenuProps> = React.memo(({graphId, onDelete, ...modalProps}) => {
  const [deleteGraph] = graphsApi.useDeleteGraphMutation();

  return (
    <Modal {...modalProps} centered>
      <ModalBody className="text-center">
        <h1>Delete graph?</h1>
        Do you really want to delete graph <Highlight text={graphId}/>?
      </ModalBody>

      <ButtonGroup>
        <Button
          variant="secondary"
          onClick={() => modalProps.onHide && modalProps.onHide()}
        >
          No
        </Button>

        <Button
          variant="danger"
          onClick={async () => {
            await deleteGraph(graphId).unwrap();
            onDelete && onDelete();
          }}
        >
          Yes
        </Button>
      </ButtonGroup>
    </Modal>
  );
});
DeleteMenu.displayName = "DeleteMenu";
export default DeleteMenu;