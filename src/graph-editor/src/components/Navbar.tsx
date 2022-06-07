import React, {FC} from "react";
import styles from "components/Navbar.module.scss";

const Navbar: FC<{ children: React.ReactNode }> = React.memo(({children}) => {

  return (
    <div className={styles.navBar}>
      {children}
    </div>
  );
});
Navbar.displayName = "Navbar";
export default Navbar;