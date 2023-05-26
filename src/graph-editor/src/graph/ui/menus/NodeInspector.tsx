import React, { FC } from "react";
import { GraphNode } from "graph/graphDataSlice";
import LinksInfo from "graph/ui/LinksInfo";
import Offcanvas from "components/Offcanvas";
import { OffcanvasBody, OffcanvasHeader, OffcanvasTitle } from "react-bootstrap";
import Highlight from "components/Highlight";

export interface NodeInspectorProps {
    node?: GraphNode;
    visible?: boolean;
    onClose?: () => void;
    onNodeClick?: (nodeId: string) => void;
    onRemoveLinkClick: (linkId: string) => void;
}

const formatNumber = (num: number) => num.toLocaleString("en-US", {
    maximumFractionDigits: 2,
    minimumFractionDigits: 2
});

const NodeInspector: FC<NodeInspectorProps> = React.memo(({ node, onClose, visible, onNodeClick, onRemoveLinkClick }) => {
    if (!node)
        return null;

    return (
        <Offcanvas position="left" show={visible} topOffset={32}>
            <OffcanvasHeader closeButton onHide={onClose} className="pb-0" closeVariant="white">
                <OffcanvasTitle><Highlight text={node.name} /></OffcanvasTitle>
            </OffcanvasHeader>
            <span className="ms-3">
                {node.nodeClass && <span><Highlight text={node.nodeClass} /></span>}
                <span className="ms-1 text-muted">({formatNumber(node.x)}, {formatNumber(node.y)})</span>
            </span>

            <OffcanvasBody>
                <LinksInfo nodeId={node.id} onNodeClick={onNodeClick} onRemoveClick={onRemoveLinkClick} />
            </OffcanvasBody>
        </Offcanvas>
    );
});

NodeInspector.displayName = "NodeInspector";
export default NodeInspector;