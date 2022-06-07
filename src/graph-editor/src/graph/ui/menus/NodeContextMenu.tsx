import React, {FC} from "react";
import ContextMenu, {ContextMenuProps} from "components/ContextMenu";
import {ListGroup} from "react-bootstrap";

export interface NodeContextMenuProps extends ContextMenuProps {
  onAddLinkClick?: () => void;
  onDeleteNodeClick?: () => void;
}

const NodeContextMenu: FC<NodeContextMenuProps> =
  React.memo(({onAddLinkClick, onDeleteNodeClick, ...cmProps}) => {

    return (
      <ContextMenu {...cmProps}>
        <ListGroup>
          <ListGroup.Item action onClick={onAddLinkClick}>
            Add link
          </ListGroup.Item>
          <ListGroup.Item action onClick={onDeleteNodeClick}>
            Delete node
          </ListGroup.Item>
        </ListGroup>
      </ContextMenu>
    );
  });
NodeContextMenu.displayName = "NodeContextMenu";
export default NodeContextMenu;