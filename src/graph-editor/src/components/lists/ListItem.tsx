import React, { FC } from "react";
import styles from "components/lists/ListItem.module.scss";

export interface ListItemProps {
    children?: React.ReactNode;
    onChildrenClick?: () => void;
    onRemoveClick?: () => void;
}

const ListItem: FC<ListItemProps> =
    React.memo(({ children, onChildrenClick, onRemoveClick }) => {

        return (
            <div className={styles.item}>
                <div className={styles.children} onClick={() => onChildrenClick && onChildrenClick()}>
                    {children}
                </div>
                <button onClick={() => onRemoveClick && onRemoveClick()}>
                    <div className="d-flex align-items-center justify-content-center">
                        <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor"
                            className="bi bi-x-lg" viewBox="0 0 16 16">
                            <path
                                d="M2.146 2.854a.5.5 0 1 1 .708-.708L8 7.293l5.146-5.147a.5.5 0 0 1 .708.708L8.707 8l5.147 5.146a.5.5 0 0 1-.708.708L8 8.707l-5.146 5.147a.5.5 0 0 1-.708-.708L7.293 8 2.146 2.854Z" />
                        </svg>
                    </div>
                </button>
            </div>
        );
    });
ListItem.displayName = "ListItem";
export default ListItem;