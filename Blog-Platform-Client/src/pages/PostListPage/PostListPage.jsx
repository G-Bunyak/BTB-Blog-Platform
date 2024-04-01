import React, { useContext, useEffect, useState } from "react";
import classes from "./PostListPage.module.css";
import Modal from "../../components/Modal/Modal.jsx";
import PostsAPI from "../../services/PostsService.js";
import { AuthContext } from "../../context/AuthContext.js";
import PostForm from "../../components/PostForm/PostForm.jsx";
import { useNavigate } from "react-router-dom";
import PostList from "../../components/PostList/PostList.jsx";

const PostListPage = () => {
  const [isModalVisible, setIsModalVisible] = useState(false);
  const [posts, setPosts] = useState([]);

  const { token, setToken } = useContext(AuthContext);

  const navigate = useNavigate();

  useEffect(() => {
    getPostsFromServer();
  }, []);

  async function getPostsFromServer() {
    let serverPosts = await PostsAPI.getPosts();
    if (serverPosts) {
      setPosts(serverPosts.posts);
    }
  }

  const createOperation = async (newPost) => {
    setIsModalVisible(false);

    let postResult = await PostsAPI.createPost(newPost);
    if (postResult.status === 200 && postResult?.data?.post) {
      setPosts([...posts, postResult.data.post]);
    }

    if (postResult.status === 401) {
      setToken("");
      localStorage.setItem("token", "");
      navigate(`/login`);
    }
  };

  const removePost = async (postId) => {
    let deleteResult = await PostsAPI.deletePost(postId);
    if (deleteResult.status === 200) {
      let newPostsArray = posts.filter((value) => value.id !== postId);
      setPosts(newPostsArray);
    }

    if (deleteResult.status === 401) {
      setToken("");
      localStorage.setItem("token", "");
      navigate(`/login`);
    }
  };

  return (
    <div>
      <Modal visible={isModalVisible} setVisible={setIsModalVisible}>
        <PostForm post={null} operation={createOperation} />
      </Modal>
      <PostList posts={posts} removeFunction={removePost} />
      <div className={classes.add_button_div}>
        <button
          onClick={() => {
            setIsModalVisible(true);
          }}
          className={classes.add_button}
        >
          <p className={classes.add_text}>+</p>
        </button>
      </div>
    </div>
  );
};

export default PostListPage;
