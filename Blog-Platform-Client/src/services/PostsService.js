import axios from "axios";

export default class PostsAPI {
  static async getPosts() {
    try {
      const response = await axios.get("https://localhost:7282/api/post");
      return response.data;
    } catch (e) {
      console.log(e.message);
    }
  }

  static async getPostDetails(id) {
    try {
      const response = await axios.get(`https://localhost:7282/api/post/${id}`);
      return response.data;
    } catch (e) {
      console.log(e.message);
    }
  }

  static async createPost(post) {
    const config = {
      headers: {
        Authorization: `Bearer ${localStorage.getItem("token")}`,
      },
    };

    try {
      const response = await axios.post(
        `https://localhost:7282/api/post`,
        post,
        config
      );

      return response;
    } catch (e) {
      if (e.message == "Request failed with status code 401") {
        let errorResponse = { status: 401 };
        return errorResponse;
      }

      console.log(e.message);
    }
  }

  static async postAddPostComment(postId, comment) {
    const config = {
      headers: {
        Authorization: `Bearer ${localStorage.getItem("token")}`,
      },
    };

    try {
      const response = await axios.post(
        `https://localhost:7282/api/post/${postId}/comment`,
        comment,
        config
      );
      return response;
    } catch (e) {
      if (e.message == "Request failed with status code 401") {
        let errorResponse = { status: 401 };
        return errorResponse;
      }

      console.log(e.message);
    }
  }

  static async deletePostComment(postId, commentId) {
    const config = {
      headers: {
        Authorization: `Bearer ${localStorage.getItem("token")}`,
      },
    };

    try {
      const response = await axios.delete(
        `https://localhost:7282/api/post/${postId}/comment/${commentId}`,
        config
      );
      return response;
    } catch (e) {
      if (e.message == "Request failed with status code 401") {
        let errorResponse = { status: 401 };
        return errorResponse;
      }

      console.log(e.message);
    }
  }

  static async updatePostComment(postId, comment) {
    const config = {
      headers: {
        Authorization: `Bearer ${localStorage.getItem("token")}`,
      },
    };

    try {
      const response = await axios.put(
        `https://localhost:7282/api/post/${postId}/comment/${comment.id}`,
        comment,
        config
      );
      return response;
    } catch (e) {
      if (e.message == "Request failed with status code 401") {
        let errorResponse = { status: 401 };
        return errorResponse;
      }

      console.log(e.message);
    }
  }

  static async updatePost(post) {
    const config = {
      headers: {
        Authorization: `Bearer ${localStorage.getItem("token")}`,
      },
    };

    try {
      const response = await axios.put(
        `https://localhost:7282/api/post/${post.id}`,
        post,
        config
      );
      return response;
    } catch (e) {
      if (e.message == "Request failed with status code 401") {
        let errorResponse = { status: 401 };
        return errorResponse;
      }

      console.log(e.message);
    }
  }

  static async deletePost(postId) {
    const config = {
      headers: {
        Authorization: `Bearer ${localStorage.getItem("token")}`,
      },
    };

    try {
      const response = await axios.delete(
        `https://localhost:7282/api/post/${postId}`,
        config
      );
      return response;
    } catch (e) {
      if (e.message == "Request failed with status code 401") {
        let errorResponse = { status: 401 };
        return errorResponse;
      }

      console.log(e.message);
    }
  }

  static async getToken(credentials) {
    try {
      const response = await axios.post(
        `https://localhost:7282/api/auth`,
        credentials
      );
      return response;
    } catch (e) {
      console.log(e.message);
    }
  }

  static async getNewUserToken(credentials) {
    try {
      const response = await axios.post(
        `https://localhost:7282/api/register`,
        credentials
      );
      return response;
    } catch (e) {
      console.log(e.message);
    }
  }
}
