import React, {FC} from "react";
import {useNavigate} from "react-router-dom";

interface ButtonProps {
  children: React.ReactNode;
  onClick?: () => void;
}

const Button: FC<ButtonProps> = React.memo((props) => {
  return (
    <div className="dropdown-item" role="button" {...props}/>
  );
});
Button.displayName = "Button";

interface LinkProps {
  children: React.ReactNode;
  href: string;
  onClick?: () => void;
}

const Link: FC<LinkProps> = React.memo((props) => {
  const navigate = useNavigate();

  return (
    <a className="dropdown-item" role="button"
       onClick={() => navigate(props.href)}
       href={props.href}
    >
      {props.children}
    </a>
  );
});
Link.displayName = "Link";

export interface ToolbarDropdownItemProps {
  children: React.ReactNode;
  href?: string;
  onClick?: () => void;
}

const ToolbarDropdownItem: FC<ToolbarDropdownItemProps>
  = React.memo(({href, ...props}) => {

  if (href)
    return <Link href={href} {...props}/>;

  return <Button {...props}/>;

});
ToolbarDropdownItem.displayName = "ToolbarDropdownItem";
export default ToolbarDropdownItem;

