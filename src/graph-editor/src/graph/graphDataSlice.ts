import { createSlice, PayloadAction } from "@reduxjs/toolkit";
import { RootState } from "store";

export interface Graph {
    id: string;
    name: string;
    graphType: string;
    data: GraphData;
}

export interface GraphNode {
    id: string;
    color: string;
    name: string;
    nodeClass: string;
    x: number;
    y: number;
}

export interface GraphLink {
    id: string;
    source: string;
    target: string;
    value: number;
}

export interface GraphData {
    nodes: GraphNode[];
    links: GraphLink[];
}

const graphDataSlice = createSlice({
    name: "graph",
    initialState: {
        id: "",
        name: "",
        graphType: "",
        data: {
            nodes: [],
            links: []
        }
    } as Graph,
    reducers: {
        setGraphData(state, action: PayloadAction<Graph>) {
            if (typeof (action.payload) === "string")
                action.payload = JSON.parse(action.payload) as Graph;
            return action.payload;
        },
        updateNode(state, action: PayloadAction<GraphNode>) {
            const nodeIndex = state.data.nodes.findIndex(x => x.id === action.payload.id);
            if (nodeIndex < 0)
                return;

            state.data.nodes[nodeIndex] = action.payload;
        },
        addNode(state, action: PayloadAction<GraphNode>) {
            state.data.nodes.push(action.payload);
        },
        deleteNode(state, { payload: nodeId }: PayloadAction<string>) {
            const nodeIndex = state.data.nodes.findIndex(x => x.id === nodeId);
            if (nodeIndex < 0)
                return;

            state.data.nodes.splice(nodeIndex, 1);
            state.data.links = state.data.links.filter(x => x.source !== nodeId && x.target !== nodeId);
        },
        addLink(state, { payload }: PayloadAction<GraphLink>) {
            if (state.data.nodes.find(x => x.id === payload.source) && state.data.nodes.find(x => x.id === payload.target)) {
                state.data.links.push({ source: payload.source, target: payload.target } as any);
            }
        },
        deleteLink(state, { payload }: PayloadAction<GraphLink>) {
            const linkIndex = state.data.links
                .findIndex(x => x.source === payload.source && x.target === payload.target);

            if (linkIndex >= 0)
                state.data.links.splice(linkIndex, 1);
        }
    }
});

export default graphDataSlice;

export const graphDataSelector = (state: RootState) => state.graph.data;

export const nodeSelector = (id: string) =>
    (state: RootState) => state.graph.data.nodes.find(x => x.id === id);

export const asTargetLinksSelector = (id: string) =>
    (state: RootState) => state.graph.data.links.filter(x => x.target === id);

export const asSourceLinksSelector = (id: string) =>
    (state: RootState) => state.graph.data.links.filter(x => x.source === id);

export const linksSelector = (state: RootState) => state.graph.data.links;
export const nodesSelector = (state: RootState) => state.graph.data.nodes;