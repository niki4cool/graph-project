import React, { FC, useEffect, useRef, useState } from "react";
import { GraphData, GraphNode } from "graph/graphDataSlice";
import ForceGraph from "react-force-graph-2d";
import styles from "./Minimap.module.scss";
import cn from "classnames";
import { ForceGraphInstance } from "force-graph";
import { useFindNode } from "graph/hooks";
import { primaryLightColor } from "vars";

export interface MinimapProps {
    graphData: GraphData;
    onNodeClick?: (node: GraphNode) => void;
    startOpened?: boolean;
}

const Minimap: FC<MinimapProps> = React.memo(({ graphData, onNodeClick, startOpened }) => {
    const graphRef = useRef<ForceGraphInstance>();
    const [show, setShow] = useState(startOpened);
    const findNode = useFindNode();

    useEffect(() => {
        graphRef.current?.zoomToFit(0);
    }, [graphData]);


    return (
        <div className={cn(styles.container, { [styles.show]: show })}>
            <div
                className={cn(styles.button, { [styles.show]: show })}
                onClick={() => setShow(!show)}
            >
                Minimap
            </div>
            <ForceGraph
                ref={graphRef}
                graphData={graphData}
                cooldownTicks={0}
                nodeColor={node => findNode(node.id)?.color || primaryLightColor}
                nodeLabel={node => findNode(node.id)?.name?.toString() || ""}
                linkColor={() => "#ffffff"}
                enableZoomInteraction={false}
                enablePanInteraction={false}
                enableNodeDrag={false}
                nodeRelSize={10}
                linkDirectionalArrowLength={20}
                linkDirectionalArrowRelPos={1}
                width={300}
                height={300}
                onNodeClick={nodeObj => {
                    const node = findNode(nodeObj.id);
                    if (node && onNodeClick)
                        onNodeClick(node);
                }}
            />
        </div>
    );
});
Minimap.displayName = "Minimap";
export default Minimap;