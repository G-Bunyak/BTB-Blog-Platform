import React, { Fragment, useContext } from "react";
import { Route, Routes } from "react-router-dom";
import ErrorPage from "../pages/ErrorPage/ErrorPage";
import PostListPage from "../pages/PostListPage/PostListPage";
import PostDetailsPage from "../pages/PostDetailsPage/PostDetailsPage";
import CommentEdit from "../pages/CommentEdit/CommentEdit";
import PostEditPage from "../pages/PostEditPage/PostEditPage";
import LoginPage from "../pages/LoginPage/LoginPage";
import { AuthContext } from "../context/AuthContext";

const AppRouter = () => {
  const { token, setToken } = useContext(AuthContext);

  return (
    <Fragment>
      {token ? (
        <Routes>
          <Route path="/post/:postId" element={<PostDetailsPage />} />
          <Route
            path="/post/:postId/comment/:commentId/edit"
            element={<CommentEdit />}
          />
          <Route path="/post/:postId/edit" element={<PostEditPage />} />
          <Route exact path="/posts" element={<PostListPage />} />
          <Route exact path="/error" element={<ErrorPage />} />
          <Route exact path="/login" element={<LoginPage />} />
          <Route path="/*" element={<PostListPage />} />
        </Routes>
      ) : (
        <Routes>
          <Route path="/post/:postId" element={<PostDetailsPage />} />
          <Route exact path="/posts" element={<PostListPage />} />
          <Route exact path="/error" element={<ErrorPage />} />
          <Route exact path="/login" element={<LoginPage />} />
          <Route path="/*" element={<LoginPage />} />
        </Routes>
      )}
    </Fragment>
  );
};

export default AppRouter;
