import React, {FC, useState} from "react";
import styles from "components/Sidebar.module.scss";

export interface SidebarProps {
  title: string;
  children: React.ReactNode;
}

const Sidebar: FC<SidebarProps> = React.memo(({title, children}) => {
  const [show, setShow] = useState(false);

  return (

    <div style={{right: show ? 0 : -300}} className={styles.container}>
      <div className={styles.button} onClick={() => setShow(!show)}>{title}</div>
      <div className={styles.content}>{children}</div>
    </div>
  );
});
Sidebar.displayName = "Sidebar";
export default Sidebar;