import classes from "./PostList.module.css";
import PostItem from "../PostItem/PostItem";

const PostList = ({ posts, removeFunction }) => {
  return (
    <div className={classes.posts_list_div}>
      <h1 className={classes.posts_list_label}>Posts list</h1>
      {posts.length > 0 ? (
        posts.map((post) => (
          <PostItem
            key={post.id}
            post={post}
            isEditClick={false}
            removeFunction={removeFunction}
          />
        ))
      ) : (
        <h1 className={classes.posts_list_label}></h1>
      )}
    </div>
  );
};

export default PostList;
