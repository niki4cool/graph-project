import { createSlice, PayloadAction } from "@reduxjs/toolkit";
import { RootState } from "store";

export interface GraphNodeMeta {
    color: string;
    type?: string;
}

export interface GraphNode {
    id: string;
    x: number;
    y: number;
    meta: GraphNodeMeta;
}

export interface GraphLink {
    source: string;
    target: string;
}

export interface GraphData {
    nodes: GraphNode[];
    links: GraphLink[];
}

const graphDataSlice = createSlice({
    name: "graphData",
    initialState: {
        nodes: [],
        links: []
    } as GraphData,
    reducers: {
        setGraphData(state, action: PayloadAction<GraphData>) {
            return action.payload;
        },
        updateNode(state, action: PayloadAction<GraphNode>) {
            const nodeIndex = state.nodes.findIndex(x => x.id === action.payload.id);
            if (nodeIndex < 0)
                return;

            state.nodes[nodeIndex] = action.payload;
        },
        addNode(state, action: PayloadAction<GraphNode>) {
            state.nodes.push(action.payload);
        },
        deleteNode(state, { payload: nodeId }: PayloadAction<string>) {
            const nodeIndex = state.nodes.findIndex(x => x.id === nodeId);
            if (nodeIndex < 0)
                return;

            state.nodes.splice(nodeIndex, 1);
            state.links = state.links.filter(x => x.source !== nodeId && x.target !== nodeId);
        },
        addLink(state, { payload }: PayloadAction<GraphLink>) {
            if (state.nodes.find(x => x.id === payload.source) && state.nodes.find(x => x.id === payload.target)) {
                state.links.push({ source: payload.source, target: payload.target } as any);
            }
        },
        deleteLink(state, { payload }: PayloadAction<GraphLink>) {
            const linkIndex = state.links
                .findIndex(x => x.source === payload.source && x.target === payload.target);

            if (linkIndex >= 0)
                state.links.splice(linkIndex, 1);
        }
    }
});

export default graphDataSlice;

export const graphDataSelector = (state: RootState) => state.graphData;

export const nodeSelector = (id: string) =>
    (state: RootState) => state.graphData.nodes.find(x => x.id === id);

export const asTargetLinksSelector = (id: string) =>
    (state: RootState) => state.graphData.links.filter(x => x.target === id);

export const asSourceLinksSelector = (id: string) =>
    (state: RootState) => state.graphData.links.filter(x => x.source === id);

export const linksSelector = (state: RootState) => state.graphData.links;