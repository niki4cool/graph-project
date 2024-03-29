﻿import React, { FC, useEffect, useRef, useState } from "react";
import ForceGraph, { NodeObject } from "react-force-graph-2d";
import { ForceGraphInstance } from "force-graph";
import { useAppDispatch, useAppSelector } from "store";
import graphDataSlice, { GraphNode, linksSelector, nodeSelector } from "graph/graphDataSlice";
import styles from "./Graph.module.css";
import { useFindNode, useGraphDataCopy } from "graph/hooks";
import AddNodeContextMenu from "graph/ui/menus/AddNodeContextMenu";
import NodeInspector from "graph/ui/menus/NodeInspector";
import AddLinkHelpMessage from "graph/ui/AddLinkHelpMessage";
import NodeContextMenu from "graph/ui/menus/NodeContextMenu";
import { ContextMenuPosition } from "components/ContextMenu";
import { useWindowSize } from "hooks";
import { Dropdown } from "react-bootstrap";
import Toolbar from "components/toolbar/Toolbar";
import ToolbarDropdown from "components/toolbar/ToolbarDropdown";
import ToolbarDropdownItem from "components/toolbar/ToolbarDropdownItem";
import DeleteMenu from "graph/ui/menus/DeleteMenu";
import ToolbarItem from "components/toolbar/ToolbarItem";
import HelpMenu from "graph/ui/menus/HelpMenu";
import NodesListMenu from "graph/ui/menus/NodesListMenu";
import CopyLabel from "components/CopyLabel";
import Minimap from "graph/ui/Minimap";
import { primaryLightColor } from "vars";
import { HubConnection } from "@microsoft/signalr";
import { useNavigate } from "react-router-dom";

export interface GraphProps {
    graphId: string;
    connection: HubConnection;
}

const Graph: FC<GraphProps> = React.memo(({ graphId, connection }) => {
    const navigate = useNavigate();
    const graphRef = useRef<ForceGraphInstance>();
    const dispatch = useAppDispatch();
    const graphData = useGraphDataCopy();
    const findNode = useFindNode();
    const links = useAppSelector(linksSelector);

    const [addNodeCmPosition, setAddNodeCmPosition] = useState<ContextMenuPosition>();
    const [addNodePosition, setAddNodePosition] = useState({ x: 0, y: 0 });

    const [leftClickedNode, setLeftClickedNode] = useState<GraphNode>();
    const [inspectorShow, setInspectorShow] = useState(false);
    const inspectorNode = useAppSelector(nodeSelector(leftClickedNode?.id || ""));

    const [addLinkSelectedNode, setAddLinkSelectedNode] = useState<GraphNode>();
    const [addingLink, setAddingLink] = useState(false);
    const addLink = (target: GraphNode) => {
        const source = addLinkSelectedNode;
        if (!source || !target)
            return;

        const linkExist = links.find(x => x.source === source.id && x.target === target.id);
        const selfLink = source.id === target.id;
        if (linkExist || selfLink)
            return;

        connection?.send("Connect", { FromId: source.id, ToId: target.id });

        resetLinkAdding();
    };
    const resetLinkAdding = () => {
        setAddLinkSelectedNode(undefined);
        setAddingLink(false);
    };

    const [nodeCmPosition, setNodeCmPosition] = useState<ContextMenuPosition>();
    const [rightClickedNode, setRightClickedNode] = useState<GraphNode>();

    const [width, height] = useWindowSize();

    const lookAtNode = (node: GraphNode) => {
        graphRef.current?.centerAt(node.x, node.y, 200);
    };

    const [deleteMenuShow, setDeleteMenuShow] = useState(false);
    const [helpMenuShow, setHelpMenuShow] = useState(true);

    const [fitted, setFitted] = useState(false);
    useEffect(() => {
        if (!graphData?.nodes.length || fitted)
            return;
        graphRef.current?.zoomToFit(0);
        setFitted(true);
    }, [fitted, graphData]);

    return (
        <>
            <h1 onClick={() => navigate("../")} className={styles.header}>Graph Editor</h1>
            <Toolbar>
                <ToolbarDropdown title="Граф">
                    <Dropdown.Menu>
                        <Dropdown.Header className="text-primary">
                            <CopyLabel copyValue={window.location.href}>{graphId}</CopyLabel>
                        </Dropdown.Header>
                        <Dropdown.Divider />
                        <ToolbarDropdownItem onClick={() => setDeleteMenuShow(true)}>
                            Удалить
                        </ToolbarDropdownItem>
                    </Dropdown.Menu>
                </ToolbarDropdown>
                <ToolbarItem onClick={() => graphRef.current?.zoomToFit(500)}>Масштабировать до графа</ToolbarItem>
                <ToolbarItem onClick={() => setHelpMenuShow(true)}>Помощь</ToolbarItem>
            </Toolbar>

            <DeleteMenu
                graphId={graphId}
                show={deleteMenuShow}
                onHide={() => setDeleteMenuShow(false)}
            />
            <HelpMenu show={helpMenuShow} onHide={() => setHelpMenuShow(false)} />
            <NodesListMenu
                nodes={graphData?.nodes || ([] as GraphNode[])}
                onNodeClick={lookAtNode}
                onNodeDelete={node => {
                    connection?.send("RemoveNode", {
                        NodeId: node.id
                    });
                }
                }
            />

            <Minimap startOpened graphData={graphData!} onNodeClick={lookAtNode} />

            <div className={styles.graph}>
                <ForceGraph
                    ref={graphRef}
                    graphData={graphData}
                    cooldownTicks={0}
                    nodeColor={node => findNode(node.id)?.color || primaryLightColor}
                    nodeLabel={node => findNode(node.id)?.name?.toString() || ""}
                    linkColor={() => "#ffffff"}
                    linkWidth={() => 2}
                    nodeRelSize={10}
                    linkDirectionalArrowLength={20}
                    linkDirectionalArrowRelPos={1}
                    width={width}
                    height={height}
                    minZoom={0.5}
                    maxZoom={5}

                    onNodeDragEnd={(nodeObj: NodeObject) => {
                        const node = findNode(nodeObj.id);
                        if (!node)
                            return;
                        console.log(nodeObj);

                        connection?.send("SetNodeMeta", {
                            NodeId: nodeObj.id,
                            Meta: {
                                X: nodeObj.x,
                                Y: nodeObj.y,
                                Name: findNode(node.id)?.name?.toString(),
                                NodeClass: node.nodeClass || "",
                                Color: node.color
                            }
                        });
                    }}

                    onBackgroundRightClick={(event => {
                        setAddNodeCmPosition({ top: event.y, left: event.x });
                        const graphCoords = graphRef.current?.screen2GraphCoords(event.x, event.y);
                        setAddNodePosition(graphCoords!);
                    })}

                    onNodeClick={clickedNode => {
                        const node = findNode(clickedNode.id);
                        if (!node)
                            return;

                        if (addingLink) {
                            addLink(node);
                        } else {
                            setLeftClickedNode(node);
                            setInspectorShow(true);
                        }
                    }}

                    onNodeRightClick={(clickedNode, event) => {
                        const node = findNode(clickedNode.id);
                        if (!node)
                            return;

                        setNodeCmPosition({ top: event.y, left: event.x });
                        setRightClickedNode(node);
                    }}
                />
            </div>

            <NodeContextMenu
                position={nodeCmPosition}
                onOutsideClick={() => setNodeCmPosition(undefined)}
                onAddLinkClick={() => {
                    setNodeCmPosition(undefined);
                    setAddingLink(true);
                    setAddLinkSelectedNode(rightClickedNode);
                }}
                onDeleteNodeClick={() => {
                    setNodeCmPosition(undefined);
                    if (rightClickedNode) {
                        connection?.send("RemoveNode", {
                            NodeId: rightClickedNode.id
                        });
                    }
                }}
            />
            <AddLinkHelpMessage visible={addingLink} onCancel={resetLinkAdding} />
            <AddNodeContextMenu
                position={addNodeCmPosition}
                onOutsideClick={() => setAddNodeCmPosition(undefined)}
                onAdd={formData => {

                    connection?.send("CreateNode", {
                        Meta: {
                            X: addNodePosition.x,
                            Y: addNodePosition.y,
                            Name: formData.id,
                            NodeClass: formData.type || "",
                            Color: formData.color
                        }
                    });

                    setAddNodeCmPosition(undefined);
                }}
            />
            <NodeInspector
                node={inspectorNode}
                onClose={() => setInspectorShow(false)}
                visible={inspectorShow}
                onNodeClick={id => {
                    const node = findNode(id);
                    if (node)
                        lookAtNode(node);
                }}
                onRemoveLinkClick={linkId => {
                    connection?.send("RemoveEdge", {
                        EdgeId: linkId
                    });
                }}
            />
        </>
    );
});
Graph.displayName = "Graph";
export default Graph;