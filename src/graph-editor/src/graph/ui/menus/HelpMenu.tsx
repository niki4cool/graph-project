import React, { FC } from "react";
import { Button, Col, Modal, ModalBody, ModalFooter, ModalProps, Row } from "react-bootstrap";
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
    = React.memo(({ header, content, icon }) => {

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
                <Modal.Title>Добро пожаловать!</Modal.Title>
            </Modal.Header>
            <ModalBody className={styles.body}>
                <Row xs={2} className={"g-4"}>
                    <Feature
                        header="Создание вершин"
                        content="Нажмите ПКМ по фону чтобы создаеть новую вершину."
                        icon={<PlusCircleDotted width={iconSize} height={iconSize} />}
                    />
                    <Feature
                        header="Удаление вершин"
                        content="Нажмите ПКМ по вершине и выберите Удалить или используйте меню вершин справа."
                        icon={<DashCircleDotted width={iconSize} height={iconSize} />}
                    />
                    <Feature
                        header="Данные вершины"
                        content="Нажмите ЛКМ по вершине чтобы увидеть детали."
                        icon={<Search width={iconSize} height={iconSize} />}
                    />
                    {/*  </Row>
        <Row>*/}
                    <Feature
                        header="Перемещение вершин"
                        content="Вершины можно перемещать зажав ЛКМ"
                        icon={<ArrowsMove width={iconSize} height={iconSize} />}
                    />
                    <Feature
                        header="Создание ребер"
                        content="Нажмите ПКМ на вершине и выберите Добавить ссылку."
                        icon={<NodePlus width={iconSize} height={iconSize} />}
                    />
                    <Feature
                        header="Удаление ребер"
                        content="Нажмите ЛКМ на вершине и используйте меню ребер слева."
                        icon={<NodeMinus width={iconSize} height={iconSize} />}
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