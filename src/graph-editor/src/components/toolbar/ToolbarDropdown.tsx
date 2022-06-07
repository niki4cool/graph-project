import React, {FC} from "react";
import {Dropdown} from "react-bootstrap";
import {ToolbarDropdownToggle} from "components/toolbar/ToolbarDropdownToggle";
import styles from "./Toolbar.module.scss";

export interface ToolbarDropdownProps {
  title: string;
  children?: React.ReactNode;
}

const ToolbarDropdown: FC<ToolbarDropdownProps>
  = React.memo(({title, children}) => {

  return (
    <Dropdown className={styles.dropdown}>
      <Dropdown.Toggle as={ToolbarDropdownToggle}>
        {title}
      </Dropdown.Toggle>
      {children}
    </Dropdown>
  );
});
ToolbarDropdown.displayName = "ToolbarDropdown";
export default ToolbarDropdown;