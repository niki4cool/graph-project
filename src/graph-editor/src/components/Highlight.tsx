import React, {FC} from "react";
import styles from "./Highlight.module.scss";

export interface HighlightProps {
  text?: string;
}

const Highlight: FC<HighlightProps> = React.memo(({text}) => {

  return (
    <span className={styles.highlight}>{text}</span>
  );
});
Highlight.displayName = "Highlight";
export default Highlight;