import React from "react";
import classes from "./CommentList.module.css";
import CommentItem from "../CommentItem/CommentItem";

const CommentList = ({ comments, removeFunction, editFunction }) => {
  return (
    <div className={classes.comments_list_div}>
      <h2 className={classes.comments_list_label}>Comments</h2>
      {comments ? (
        comments.map((comment) => (
          <CommentItem
            key={comment.id}
            comment={comment}
            removeFunction={removeFunction}
          />
        ))
      ) : (
        <h2 className={classes.comments_list_label}>Loading...</h2>
      )}
    </div>
  );
};

export default CommentList;
