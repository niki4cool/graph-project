import React from "react";
import styles from "components/lists/List.module.scss";
import ListHeader from "components/lists/ListHeader";

export interface ListProps<TItem> extends React.HTMLProps<HTMLDivElement> {
  items: TItem[];
  header?: string;
  emptyHeader?: string;
  renderItem: (item: TItem) => React.ReactNode;
}

export function List<TItem>(
  {emptyHeader, header, items, renderItem, ...divProps}: ListProps<TItem>) {

  return (
    <div {...divProps} className={`${styles.list} ${divProps.className || ""}`}>
      {items.length === 0
        ? <ListHeader>{emptyHeader}</ListHeader>
        :
        <>
          <ListHeader>{header}</ListHeader>
          {items.map(item => renderItem(item))}
        </>
      }
    </div>
  );
}

export default List;