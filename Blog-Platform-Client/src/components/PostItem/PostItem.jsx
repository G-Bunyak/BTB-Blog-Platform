import React from "react";
import classes from "./PostItem.module.css";
import CustomText from "../baseComponents/CustomText/CustomText";
import { useNavigate } from "react-router-dom";

const PostItem = ({ post, removeFunction, isEditClick }) => {
  const navigate = useNavigate();

  async function redirectClick() {
    if (isEditClick) {
      navigate(`/post/${post.id}/edit`);
    } else {
      navigate(`/post/${post.id}`);
    }
  }

  async function removePost(e) {
    e.stopPropagation();
    removeFunction(post.id);
  }

  return (
    <div className={classes.post_item_div} onClick={redirectClick}>
      <div className={classes.post_div}>
        <div className={classes.post_title_div}>
          <CustomText size={"big"}>{post.title}</CustomText>
          <div className={classes.post_author_nickname}>
            <CustomText size={"small"}>by {post.authorNickname}</CustomText>
          </div>
        </div>
        <CustomText size={"medium"}>{post.content}</CustomText>
      </div>
      {!isEditClick ? (
        <div className={classes.post_control_div}>
          <button
            onClick={(e) => {
              removePost(e);
            }}
            className={classes.remove_button}
          >
            <p className={classes.remove_button_text}>X</p>
          </button>
        </div>
      ) : (
        <div></div>
      )}
    </div>
  );
};

export default PostItem;
