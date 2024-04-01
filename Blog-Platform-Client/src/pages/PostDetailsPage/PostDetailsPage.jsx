import React, { Fragment, useContext, useEffect, useState } from "react";
import PostsAPI from "../../services/PostsService";
import { useNavigate, useParams } from "react-router-dom";
import CustomText from "../../components/baseComponents/CustomText/CustomText";
import PostItem from "../../components/PostItem/PostItem";
import classes from "./PostDetailsPage.module.css";
import CommentList from "../../components/CommentList/CommentList";
import Modal from "../../components/Modal/Modal";
import CommentForm from "../../components/CommentForm/CommentForm";
import { AuthContext } from "../../context/AuthContext";

const PostDetailsPage = () => {
  const { postId } = useParams();

  const [post, setPost] = useState();
  const [isPostFound, setIsPostFound] = useState(true);
  const [comments, setComments] = useState([]);

  const [isCommentModalVisible, setIsCommentModalVisible] = useState(false);

  const { token, setToken } = useContext(AuthContext);

  const navigate = useNavigate();

  useEffect(() => {
    getPostDataFromServer();
  }, []);

  async function getPostDataFromServer() {
    if (postId) {
      let postDetails = await PostsAPI.getPostDetails(postId);
      if (postDetails) {
        setPost(postDetails.post);
        setComments(postDetails.comments);
      } else {
        setIsPostFound(false);
      }
    }
  }

  const commentOperation = async (comment) => {
    setIsCommentModalVisible(false);

    let postResult = await PostsAPI.postAddPostComment(postId, comment);
    if (postResult.status === 200 && postResult?.data?.comment) {
      setComments([...comments, postResult.data.comment]);
    }

    if (postResult.status === 401) {
      setToken("");
      localStorage.setItem("token", "");
      navigate(`/login`);
    }
  };

  const removeComment = async (commentId) => {
    let deleteResult = await PostsAPI.deletePostComment(postId, commentId);
    if (deleteResult.status === 200) {
      let newCommentsArray = comments.filter((value) => value.id !== commentId);
      setComments(newCommentsArray);
    }

    if (deleteResult.status === 401) {
      setToken("");
      localStorage.setItem("token", "");
      navigate(`/login`);
    }
  };

  return (
    <div>
      {post ? (
        <Fragment>
          <Modal
            visible={isCommentModalVisible}
            setVisible={setIsCommentModalVisible}
          >
            <CommentForm comment={null} operation={commentOperation} />
          </Modal>
          <div className={classes.post_details_div}>
            <CustomText size={"big"}>{post.title} details</CustomText>
            <PostItem post={post} isEditClick={true} />
            <CommentList comments={comments} removeFunction={removeComment} />
          </div>
          <div className={classes.add_comment_button_div}>
            <button
              onClick={() => {
                setIsCommentModalVisible(true);
              }}
              className={classes.add_comment_button}
            >
              <p className={classes.add_comment_text}>+</p>
            </button>
          </div>
        </Fragment>
      ) : (
        <div className={classes.post_details_div}>
          <CustomText size={"big"}>
            {isPostFound ? "Loading..." : "Post not found"}
          </CustomText>
        </div>
      )}
      <div className={classes.back_button_div}>
        <button
          onClick={() => {
            navigate(`/posts`);
          }}
          className={classes.back_button}
        >
          <p className={classes.back_button_text}>←</p>
        </button>
      </div>
    </div>
  );
};

export default PostDetailsPage;
