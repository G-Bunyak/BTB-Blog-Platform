import React, { useContext, useEffect, useState } from "react";
import PostForm from "../../components/PostForm/PostForm";
import classes from "./PostEditPage.module.css";
import { useNavigate, useParams } from "react-router-dom";
import PostsAPI from "../../services/PostsService";
import { AuthContext } from "../../context/AuthContext";

const PostEditPage = () => {
  const { postId } = useParams();
  const [isLoading, setIsLoading] = useState(true);
  const [post, setPost] = useState();

  const navigate = useNavigate();

  const { token, setToken } = useContext(AuthContext);

  useEffect(() => {
    setIsLoading(true);
    getPost();
    setIsLoading(false);
  }, []);

  async function getPost() {
    if (postId) {
      let postDetails = await PostsAPI.getPostDetails(postId);
      if (postDetails?.post) {
        setPost(postDetails.post);
      }
    }
  }

  const updatePost = async (updatedPost) => {
    let response = await PostsAPI.updatePost(updatedPost);
    if (response.status === 401) {
      setToken("");
      localStorage.setItem("token", "");
      navigate(`/login`);
    } else {
      navigate(`/post/${postId}`);
    }
  };

  return (
    <div className={classes.post_form_div}>
      {isLoading ? (
        <h1 className={classes.post_form_label}>Loading...</h1>
      ) : post ? (
        <div>
          <PostForm post={post} operation={updatePost} />
        </div>
      ) : (
        <h1 className={classes.post_form_label}>Post not found</h1>
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

export default PostEditPage;
