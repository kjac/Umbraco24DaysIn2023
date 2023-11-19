import { BrowserRouter, Routes, Route } from "react-router-dom"
import ScrollToTop from "./ScrollToTop";
import Home from "./Home"
import Post from "./Post"
import Author from "./Author";
import Tag from "./Tag";

function App() {
  return (
      <BrowserRouter>
        <ScrollToTop />
        <Routes>
          <Route path="/" element={<Home />} />
          <Route path="/posts/*" element={<Post />} />
          <Route path="/authors/*" element={<Author />} />
          <Route path="/tags/:tag" element={<Tag />} />
        </Routes>
      </BrowserRouter>
  )
}

export default App;
