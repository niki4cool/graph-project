import React, {FC} from "react";
import {Button, Col, Modal, ModalBody, ModalFooter, ModalProps, Row} from "react-bootstrap";
import styles from "./HelpMenu.module.scss";
import {
  ArrowsMove,
  DashCircleDotted,
  NodeMinus,
  NodePlus,
  PlusCircleDotted,
  Search
} from "components/Icons";

const Feature: FC<{ header: string, content: string, icon: React.ReactNode }>
  = React.memo(({header, content, icon}) => {

  return (
    <Col className={styles.feature}>
      <div className={styles.icon}>
        {icon}
      </div>
      <div>
        <h1>{header}</h1>
        <p>{content}</p>
      </div>
    </Col>
  );
});
Feature.displayName = "Feature";

const HelpMenu: FC<ModalProps> = React.memo((props) => {
  const iconSize = "2rem";

  return (
    <Modal fullscreen="lg-down" size="lg" {...props} centered >
      <Modal.Header closeButton closeVariant="white">
        <Modal.Title>Welcome to Graph Editor</Modal.Title>
      </Modal.Header>
      <ModalBody className={styles.body}>
        <Row xs={2} className={"g-4"}>
          <Feature
            header="Create nodes"
            content="Right click on background to create a new node."
            icon={<PlusCircleDotted width={iconSize} height={iconSize}/>}
          />
          <Feature
            header="Delete nodes"
            content="Right click on node and select Delete or use Nodes menu on the right."
            icon={<DashCircleDotted width={iconSize} height={iconSize}/>}
          />
          <Feature
            header="Inspect nodes"
            content="Left click on node to see details."
            icon={<Search width={iconSize} height={iconSize}/>}
          />
      {/*  </Row>
        <Row>*/}
          <Feature
            header="Move nodes"
            content="Drag node to change it position."
            icon={<ArrowsMove width={iconSize} height={iconSize}/>}
          />
          <Feature
            header="Add links"
            content="Right click on node and select Add link."
            icon={<NodePlus width={iconSize} height={iconSize}/>}
          />
          <Feature
            header="Remove links"
            content="Left click on node and click X near the required link."
            icon={<NodeMinus width={iconSize} height={iconSize}/>}
          />
        </Row>
      </ModalBody>
      <ModalFooter>
        <Button variant="light" onClick={props.onHide}>Got it!</Button>
      </ModalFooter>
    </Modal>
  );
});
HelpMenu.displayName = "HelpMenu";
export default HelpMenu;