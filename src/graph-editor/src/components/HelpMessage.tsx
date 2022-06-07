import React, {FC} from "react";
import styles from "components/HelpMessage.module.scss";
import {CloseButton} from "react-bootstrap";

export interface HelpMessageProps {
  children?: React.ReactNode;
  visible?: boolean;
  cancelButton?: boolean;
  onCancel?: () => void;
}

const HelpMessage: FC<HelpMessageProps> =
  React.memo(({children, visible, cancelButton, onCancel}) => {
    if (!visible)
      return null;

    return (
      <div className={styles.container}>
        <div className={styles.content}>
          {children}
          {cancelButton && <CloseButton onClick={onCancel} className={styles.closeButton}/>}
        </div>
      </div>
    );
  });
HelpMessage.displayName = "HelpMessage";
export default HelpMessage;