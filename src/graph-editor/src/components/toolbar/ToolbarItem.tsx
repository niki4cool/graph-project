import React, {FC} from "react";
import styles from "components/toolbar/Toolbar.module.scss";

const ToolbarItem: FC<React.HTMLProps<HTMLSpanElement>> = React.memo((props) => {

  return (
    <span className={styles.item} {...props}/>
  );
});
ToolbarItem.displayName = "ToolbarItem";
export default ToolbarItem;