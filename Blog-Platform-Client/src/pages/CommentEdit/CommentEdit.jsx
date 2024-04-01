import React, { useContext, useEffect, useState } from "react";
import CommentForm from "../../components/CommentForm/CommentForm";
import classes from "./CommentEdit.module.css";
import { useNavigate, useParams } from "react-router-dom";
import PostsAPI from "../../services/PostsService";
import { AuthContext } from "../../context/AuthContext";

const CommentEdit = () => {
  const { postId, commentId } = useParams();
  const [isLoading, setIsLoading] = useState(true);
  const [comment, setComment] = useState();

  const { token, setToken } = useContext(AuthContext);

  const navigate = useNavigate();

  useEffect(() => {
    setIsLoading(true);
    getComment();
    setIsLoading(false);
  }, []);

  async function getComment() {
    if (postId) {
      let postDetails = await PostsAPI.getPostDetails(postId);
      if (postDetails.comments) {
        let commentToEdit = postDetails.comments.find((c) => c.id == commentId);
        if (commentToEdit) {
          setComment(commentToEdit);
        }
      }
    }
  }

  const updateComment = async (updatedComment) => {
    let response = await PostsAPI.updatePostComment(postId, updatedComment);
    if (response.status === 401) {
      setToken("");
      localStorage.setItem("token", "");
      navigate(`/login`);
    } else {
      navigate(`/post/${postId}`);
    }
  };

  return (
    <div className={classes.comments_form_div}>
      {isLoading ? (
        <h1 className={classes.comments_form_label}>Loading...</h1>
      ) : comment ? (
        <div>
          <CommentForm comment={comment} operation={updateComment} />
        </div>
      ) : (
        <h1 className={classes.comments_form_label}>Comment not found</h1>
      )}
      <div className={classes.back_button_div}>
        <button
          onClick={() => {
            navigate(`/post/${postId}`);
          }}
          className={classes.back_button}
        >
          <p className={classes.back_button_text}>←</p>
        </button>
      </div>
    </div>
  );
};

export default CommentEdit;
