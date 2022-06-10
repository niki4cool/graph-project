import React, {FC} from "react";
import {useFindNode} from "graph/hooks";

export interface NodeLongNameProps {
  nodeId: string;
}

const NodeLongName: FC<NodeLongNameProps> = React.memo(({nodeId}) => {
  const node = useFindNode()(nodeId);
  if (!node)
    return null;


  return (
    <span>{node.id} {node.meta.type &&<span className="text-muted">({node.meta.type})</span>}</span>
  );
});
NodeLongName.displayName = "NodeLongName";
export default NodeLongName;