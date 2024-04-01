import React from "react";
import classes from "./CustomText.module.css";

const CustomText = ({ children, size, ...props }) => {
  const divClasses = [classes.custom_text];
  if (size) {
    switch (size) {
      case "big":
        divClasses.push(classes.size_big);
        break;
      case "medium":
        divClasses.push(classes.size_medium);
        break;
      case "small":
        divClasses.push(classes.size_small);
        break;
      default:
        divClasses.push(classes.size_medium);
        break;
    }
  }

  return (
    <p {...props} className={divClasses.join(" ")}>
      {children}
    </p>
  );
};

export default CustomText;
