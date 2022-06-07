import React, {FC} from "react";
import styles from "components/toolbar/Toolbar.module.scss";

export interface ToolbarProps {
  children: React.ReactNode;
}

const Toolbar: FC<ToolbarProps> = React.memo(({children}) => {

  return (
    <div className={styles.toolbar}>
      {children}
    </div>
  );
});
Toolbar.displayName = "Toolbar";
export default Toolbar;