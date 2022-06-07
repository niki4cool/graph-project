import React, {FC, useEffect, useRef, useState} from "react";
import ForceGraph from "react-force-graph-2d";
import {ForceGraphInstance} from "force-graph";
import {useWindowSize} from "hooks";

function genRandomTree(N = 300, reverse = false) {
  return {
    nodes: Array.from(Array(N).keys()).map(i => ({id: i})),
    links: Array.from(Array(N).keys())
      .filter(id => id)
      .map(id => ({
        [reverse ? "target" : "source"]: id,
        [reverse ? "source" : "target"]: Math.round(Math.random() * (id - 1))
      }))
  };
}

const PromoGraph: FC = React.memo(() => {
  const [width, height] = useWindowSize();
  const [graphData, setGraphData] = useState(genRandomTree(10));
  const graphRef = useRef<ForceGraphInstance>();

  useEffect(() => {
    const interval = setInterval(() => {
      setGraphData(({nodes, links}) => {
        const id = nodes.length;
        if (id > 100)
          return {nodes: nodes, links: links};

        return {
          nodes: [...nodes, {id}],
          links: [...links, {source: id, target: Math.round(Math.random() * (id - 1))}]
        };
      });
    }, 5000);
    return () => clearInterval(interval);
  }, []);

  return (
    <ForceGraph
      ref={graphRef}
      graphData={graphData}
      width={width}
      height={height}
      nodeColor={() => "#03A9F4"}
      linkColor={() => "rgba(255,255,255,0.8)"}
      nodeRelSize={5}
      cooldownTicks={100}
      onEngineStop={() => graphRef.current?.zoomToFit(5000)}
      enablePointerInteraction={true}
      enablePanInteraction={false}
      enableZoomInteraction={false}
    />
  );
});
PromoGraph.displayName = "PromoGraph";
export default PromoGraph;