import React, {FC} from "react";
import styles from "./Offcanvas.module.scss";

export interface OffcanvasProps {
  show?: boolean;
  position: "left" | "right";
  children?: React.ReactNode;
  topOffset?: number;
}

const Offcanvas: FC<OffcanvasProps> =
  React.memo(({show, children, position, topOffset = 0}) => {
    return (
      <div
        className={styles.container}
        style={
          position === "left"
            ? {left: show ? 0 : -300, top: topOffset}
            : {right: show ? 0 : -300, top: topOffset}
        }
      >
        {children}
      </div>
    );
  });
Offcanvas.displayName = "Offcanvas";
export default Offcanvas;