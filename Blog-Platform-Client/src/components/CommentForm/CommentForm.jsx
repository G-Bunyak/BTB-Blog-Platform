import React, { useState } from "react";
import { object, string } from "yup";
import classes from "./CommentForm.module.css";
import CustomInput from "../baseComponents/CustomInput/CustomInput";
import { Store } from "react-notifications-component";

const CommentForm = ({ comment, operation }) => {
  const [newComment, setNewComment] = useState(
    comment ? comment : { authorNickname: "", content: "" }
  );

  const commentSchema = object({
    content: string().required().max(65535),
    authorNickname: string().required().max(150),
  });

  async function buttonClick(e) {
    e.preventDefault();

    try {
      await commentSchema.validate(newComment);
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

    operation(newComment);

    setNewComment({ authorNickname: "", content: "" });
  }

  return (
    <div className={classes.main_div}>
      <form className={classes.form_div}>
        <h1 className={classes.comment_label}>
          {comment ? "Edit comment" : "Add comment"}
        </h1>
        <CustomInput
          value={newComment.authorNickname}
          onChange={(e) =>
            setNewComment({ ...newComment, authorNickname: e.target.value })
          }
          type="text"
          placeholder="Nickname"
        />
        <CustomInput
          value={newComment.content}
          onChange={(e) =>
            setNewComment({ ...newComment, content: e.target.value })
          }
          type="text"
          placeholder="Comment"
        />

        <button
          onClick={(e) => {
            buttonClick(e);
          }}
          className={classes.comment_button}
        >
          {comment ? "Edit comment" : "Add comment"}
        </button>
      </form>
    </div>
  );
};

export default CommentForm;
