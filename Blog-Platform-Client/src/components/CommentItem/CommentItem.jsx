import React from "react";
import classes from "./CommentItem.module.css";
import CustomText from "../baseComponents/CustomText/CustomText";
import { useNavigate, useParams } from "react-router-dom";

const CommentItem = ({ comment, removeFunction }) => {
  const { postId } = useParams();
  const navigate = useNavigate();

  async function removeComment(e) {
    e.stopPropagation();
    removeFunction(comment.id);
  }

  async function editComment(e) {
    e.stopPropagation();
    navigate(`/post/${postId}/comment/${comment.id}/edit`);
  }

  return (
    <div
      onClick={(e) => {
        editComment(e);
      }}
      className={classes.comment_item_div}
    >
      <div className={classes.comment_title_div}>
        <CustomText size={"small"}>{comment.authorNickname}:</CustomText>
        <CustomText size={"medium"}>{comment.content}</CustomText>
      </div>
      <div className={classes.comment_control_div}>
        <button
          onClick={(e) => {
            removeComment(e);
          }}
          className={classes.remove_button}
        >
          <p className={classes.remove_button_text}>X</p>
        </button>
      </div>
    </div>
  );
};

export default CommentItem;
