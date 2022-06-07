import React, {FC, useCallback, useEffect, useState, VFC} from "react";
import {OverlayTrigger, Tooltip} from "react-bootstrap";

import styles from "./CopyLabel.module.scss";
import classNames from "classnames";

const UpdatingTooltip: VFC<any> = React.forwardRef(
  ({popper, children, show: _, ...props}, ref) => {
    useEffect(() => {
      popper.scheduleUpdate();
    }, [children, popper]);

    return (
      <Tooltip ref={ref} {...props}>
        {children}
      </Tooltip>
    );
  }
);

export interface CopyLabelProps {
  children?: React.ReactNode;
  copyValue: string;
  className?: string;
}

const CopyLabel: FC<CopyLabelProps> = React.memo(({children, copyValue, className}) => {
  const [clicked, setClicked] = useState(false);
  const [message, setMessage] = useState("Click to copy share link");

  useEffect(() => {
    setMessage(clicked ? "Copied!" : "Click to copy share link");
  }, [clicked]);

  const handleClick = useCallback(async () => {
    setClicked(true);
    await navigator.clipboard.writeText(copyValue);
  }, [copyValue]);
  // noinspection RequiredAttributes
  return (
    <OverlayTrigger
      placement="bottom"
      delay={{show: 0, hide: 250}}
      onExited={() => setClicked(false)}
      overlay={
        <UpdatingTooltip
          id="tooltip-copy-ip"
          className={classNames({
            [`${styles.tooltipClicked}`]: clicked,
          })}
        >
          {message}
        </UpdatingTooltip>
      }
    >
      <span className={`${styles.label} ${className || ""}`} onClick={handleClick}>
        {children}
      </span>
    </OverlayTrigger>
  );
});

export default CopyLabel;
