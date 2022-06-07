import React, {FC, useRef} from "react";
import {useOutsideAlerter} from "hooks";

export interface ContextMenuPosition {
  top: number;
  left: number;
}

export interface ContextMenuProps {
  position?: ContextMenuPosition;
  children?: React.ReactNode;
  onOutsideClick?: () => void;
}

const ContextMenu: FC<ContextMenuProps> =
  React.memo(({onOutsideClick, position, children}) => {
    const ref = useRef(null);

    useOutsideAlerter(ref, () => onOutsideClick && onOutsideClick());

    if (!position)
      return null;

    return (
      <div
        ref={ref}
        style={{
          position: "fixed",
          top: position.top,
          left: position.left
        }}
      >
        {children}
      </div>
    );
  });
ContextMenu.displayName = "ContextMenu";
export default ContextMenu;