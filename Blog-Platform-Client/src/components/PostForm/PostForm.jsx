import React, { useState } from "react";
import { object, string } from "yup";
import classes from "./PostForm.module.css";
import CustomInput from "../baseComponents/CustomInput/CustomInput";
import { Store } from "react-notifications-component";

const PostForm = ({ post, operation }) => {
  const [newPost, setNewPost] = useState(
    post ? post : { authorNickname: "", title: "", content: "" }
  );

  const postSchema = object({
    content: string().required().max(65535),
    title: string().required().max(500),
    authorNickname: string().required().max(150),
  });

  async function buttonClick(e) {
    e.preventDefault();

    try {
      await postSchema.validate(newPost);
    } catch (e) {
      let message = `${e}`;
      message = message.replace("ValidationError: ", "");
      message = message[0].toUpperCase() + message.substring(1);

      Store.addNotification({
        title: "Invalid data",
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

    operation(newPost);

    setNewPost({ authorNickname: "", title: "", content: "" });
  }

  return (
    <div className={classes.main_div}>
      <form className={classes.form_div}>
        <h1 className={classes.post_label}>
          {post ? "Edit post" : "Create post"}
        </h1>
        <CustomInput
          value={newPost.authorNickname}
          onChange={(e) =>
            setNewPost({ ...newPost, authorNickname: e.target.value })
          }
          type="text"
          placeholder="Nickname"
        />
        <CustomInput
          value={newPost.title}
          onChange={(e) => setNewPost({ ...newPost, title: e.target.value })}
          type="text"
          placeholder="Title"
        />
        <CustomInput
          value={newPost.content}
          onChange={(e) => setNewPost({ ...newPost, content: e.target.value })}
          type="text"
          placeholder="Content"
        />

        <button
          onClick={(e) => {
            buttonClick(e);
          }}
          className={classes.post_button}
        >
          {post ? "Edit post" : "Create post"}
        </button>
      </form>
    </div>
  );
};

export default PostForm;
