import React, {FC} from "react";
import {GraphNode} from "graph/graphDataSlice";
import List from "components/lists/List";
import ListItem from "components/lists/ListItem";
import Sidebar from "components/Sidebar";
import NodeLongName from "graph/ui/menus/NodeLongName";

export interface NodesListMenuProps {
  nodes: GraphNode[];
  onNodeDelete?: (node: GraphNode) => void;
  onNodeClick?: (node: GraphNode) => void;
}

const NodesListMenu: FC<NodesListMenuProps> =
  React.memo(({nodes, onNodeDelete, onNodeClick}) => {
      console.log(nodes);
    return (
      <Sidebar title="Nodes">
        <List
          header="Nodes"
          emptyHeader="Graph does not contain any nodes"
          items={nodes}
          renderItem={node =>
            <ListItem
              children={<NodeLongName nodeId={node.id}/>}
              key={node.id}
              onRemoveClick={() => onNodeDelete && onNodeDelete(node)}
              onChildrenClick={() => onNodeClick && onNodeClick(node)}
            />
          }
        />
      </Sidebar>
    );
  });
NodesListMenu.displayName = "NodesListMenu";
export default NodesListMenu;