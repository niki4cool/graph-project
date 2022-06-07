import React, {FC, useEffect, useState} from "react";
import {GraphData} from "graph/graphDataSlice";
import ForceGraph from "react-force-graph-2d";

export interface GraphCanvasProps {
  graphData: GraphData;
}

const GraphCanvas: FC<GraphCanvasProps>
  = React.memo(({graphData}) => {
  const [dataCopy, setDataCopy] = useState<GraphData>();
  useEffect(() => {
    setDataCopy(structuredClone(graphData));
  }, [graphData]);


  return (
    <ForceGraph
      graphData={dataCopy}
    />
  );
});
GraphCanvas.displayName = "GraphCanvas";
export default GraphCanvas;