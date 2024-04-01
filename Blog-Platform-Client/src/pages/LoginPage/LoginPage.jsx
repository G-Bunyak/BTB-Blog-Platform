import React, { useContext, useState } from "react";
import CustomText from "../../components/baseComponents/CustomText/CustomText";
import CustomInput from "../../components/baseComponents/CustomInput/CustomInput";
import { AuthContext } from "../../context/AuthContext";
import PostsAPI from "../../services/PostsService";
import classes from "./LoginPage.module.css";
import { useNavigate } from "react-router-dom";
import { object, string } from "yup";
import { Store } from "react-notifications-component";

const LoginPage = () => {
  const [authModel, setAuthModel] = useState({ id: "", secret: "" });

  const navigate = useNavigate();

  const { token, setToken } = useContext(AuthContext);

  const loginSchema = object({
    secret: string().required().max(500),
    id: string().required().email().max(250),
  });

  async function getToken(e) {
    e.preventDefault();

    try {
      await loginSchema.validate(authModel);
    } catch (e) {
      let message = `${e}`;
      message = message.replace("ValidationError: ", "");
      message = message.replace("secret", "password");
      message = message.replace("id", "email");

      message = message[0].toUpperCase() + message.substring(1);

      Store.addNotification({
        title: "Invalid credentials",
        message: `${message}`,
        type: "danger",
        insert: "top",
        container: "top-right",
        animationIn: ["animate__animated", "animate__fadeIn"],
        animationOut: ["animate__animated", "animate__fadeOut"],
        dismiss: {
          duration: 2000,
          onScreen: true,
        },
      });
      return;
    }

    let tokenModel = await PostsAPI.getToken(authModel);
    if (tokenModel.status === 200 && tokenModel?.data?.accessToken) {
      setToken(tokenModel.data.accessToken);
      localStorage.setItem("token", tokenModel.data.accessToken);
      navigate(`/posts`);
    } else {
      Store.addNotification({
        title: "Invalid credentials",
        message: `Invalid email or password`,
        type: "danger",
        insert: "top",
        container: "top-right",
        animationIn: ["animate__animated", "animate__fadeIn"],
        animationOut: ["animate__animated", "animate__fadeOut"],
        dismiss: {
          duration: 2000,
          onScreen: true,
        },
      });
    }
  }

  async function getNewUserToken(e) {
    e.preventDefault();

    try {
      await loginSchema.validate(authModel);
    } catch (e) {
      let message = `${e}`;
      message = message.replace("ValidationError: ", "");
      message = message.replace("secret", "password");
      message = message.replace("id", "email");

      message = message[0].toUpperCase() + message.substring(1);

      Store.addNotification({
        title: "Invalid credentials",
        message: `${message}`,
        type: "danger",
        insert: "top",
        container: "top-right",
        animationIn: ["animate__animated", "animate__fadeIn"],
        animationOut: ["animate__animated", "animate__fadeOut"],
        dismiss: {
          duration: 2000,
          onScreen: true,
        },
      });
      return;
    }

    let tokenModel = await PostsAPI.getNewUserToken(authModel);
    if (tokenModel.status === 200 && tokenModel?.data?.accessToken) {
      setToken(tokenModel.data.accessToken);
      localStorage.setItem("token", tokenModel.data.accessToken);
      navigate(`/posts`);
    } else {
      Store.addNotification({
        title: "Invalid credentials",
        message: `Invalid email or password`,
        type: "danger",
        insert: "top",
        container: "top-right",
        animationIn: ["animate__animated", "animate__fadeIn"],
        animationOut: ["animate__animated", "animate__fadeOut"],
        dismiss: {
          duration: 2000,
          onScreen: true,
        },
      });
    }
  }

  return (
    <div className={classes.login_page_div}>
      <CustomText size={"big"}>Login</CustomText>
      <form className={classes.form_div}>
        <CustomInput
          value={authModel.id}
          onChange={(e) => setAuthModel({ ...authModel, id: e.target.value })}
          type="text"
          placeholder="Email"
        />
        <CustomInput
          value={authModel.secret}
          onChange={(e) =>
            setAuthModel({ ...authModel, secret: e.target.value })
          }
          type="password"
          placeholder="Password"
        />
        <div className={classes.login_button_div}>
          <button
            className={classes.login_button}
            onClick={(e) => {
              getToken(e);
            }}
          >
            <CustomText size={"medium"}>Login</CustomText>
          </button>

          <button
            className={classes.login_button}
            onClick={(e) => {
              getNewUserToken(e);
            }}
          >
            <CustomText size={"medium"}>Register</CustomText>
          </button>
        </div>
      </form>
    </div>
  );
};

export default LoginPage;
