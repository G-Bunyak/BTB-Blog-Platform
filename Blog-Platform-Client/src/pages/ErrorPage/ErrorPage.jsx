import classes from "./ErrorPage.module.css";

const ErrorPage = () => {
  return (
    <div className={classes.main_div}>
      <h1 className={classes.error_text}>Something went wrong</h1>
    </div>
  );
};

export default ErrorPage;
