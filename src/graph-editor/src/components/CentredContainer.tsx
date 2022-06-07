import React, {FC} from "react";
import {Container} from "react-bootstrap";
import styles from "./CenteredContainer.module.scss";

const CenteredContainer: FC<{ children: React.ReactNode, className?: string }> =
  React.memo(({children, className}) => {

    return (
      <Container className={`${styles.container} ${className}`}>
        {children}
      </Container>
    );
  });
CenteredContainer.displayName = "CenteredContainer";
export default CenteredContainer;