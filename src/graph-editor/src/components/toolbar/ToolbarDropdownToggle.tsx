import React from "react";
import styles from "./Toolbar.module.scss";

export const ToolbarDropdownToggle = React.forwardRef<HTMLSpanElement, React.HTMLProps<HTMLSpanElement>>
(({children, onClick}, ref) => (
  <span
    className={styles.item}
    ref={ref}
    onClick={(e) => {
      e.preventDefault();
      onClick && onClick(e);
    }}
  >
    {children}
  </span>
));