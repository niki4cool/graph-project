import React, {FC, ReactNode} from "react";

const ListHeader: FC<{ children: ReactNode }> = React.memo(({children}) => {
  return (
    <div className="bg-black text-center py-2">{children}</div>
  );
});
ListHeader.displayName = "ListHeader";
export default ListHeader;