import {FormEvent, useEffect, useState} from "react";
import {GetCurrentUser, UpdateUser} from "../api/AppApi.ts";
import '../css/ProfilePage.css';
import {useAppSelector} from "../redux/Hooks.tsx";

export function ProfilePage() {
    const user = useAppSelector((state) => state.user);
    const [userName, setUserName] = useState("");
    const [email, setEmail] = useState("");

    const [name, setName] = useState("");

    const [image, setImage] = useState<File | null>(null);
    const [imageUrl, setImageUrl] = useState<string | null>("ImgProfile/default.png");
    const [previewUrl, setPreviewUrl] = useState<string | null>(null);

    const [errorMessage, setErrorMessage] = useState<string>("");

    const [proverka, setProverka] = useState(0);

    useEffect(() => {
        console.error("Update")
        GetCurrentUser().then((res) => {
            setUserName(res.data.Name);
            setEmail(res.data.Email);
            if (res.data.ImgUrl != "" && res.data.ImgUrl != null)
            {
                setImageUrl(res.data.ImgUrl);
            }
        });
    }, [proverka]);

    const handleImageChange = (e: React.ChangeEvent<HTMLInputElement>) => {
        if (e.target.files && e.target.files[0]) {
            const selectedImage = e.target.files[0];
            setImage(selectedImage);
            setPreviewUrl(URL.createObjectURL(selectedImage)); // Обновляем предпросмотр
        }
    };

    const handleSubmit = (e: FormEvent) => {
        e.preventDefault();
        (async () => {
            try {
                const formData = new FormData();
                formData.append('UserEmail', email);
                if (name != ""){
                    formData.append('NewName', name);
                }
                if (image) {
                    formData.append('ImageFile', image);
                }

                const response = await UpdateUser(formData, user.token);
                if (!response.ok) {
                    const errorData = await response.json();
                    const error = new Error(errorData.message || `HTTP error! Status: ${response.status}`);
                    error.name = response.status.toString();
                    throw error;
                }

                setErrorMessage('');
                setProverka(proverka + 1);
                alert("Данные обновлены");
            } catch (error: any) {
                if (error.name === "400") {
                    setErrorMessage(`Ошибка при обновлении профиля. Имя должно быть от 5 символов или пустым чтобы не обновлять.`);
                } else {
                    setErrorMessage(`Произошла ошибка: ${error.message}`);
                }
                setProverka(proverka + 1);
            }
        })();
    };


    return (
        <div className="profile-page">
            <div className="profile-info">
                <div className="profile-header">
                    <h1>Профиль</h1>
                    <h3>{email}</h3>
                    <br/>
                    <h3>{userName}</h3>
                </div>
            </div>
            <div className="profile-image">
                {imageUrl ? (
                    <img
                        src={`http://localhost:5045/${imageUrl}`}
                        alt="Image"
                        style={{ width: '200px', height: '200px' }} // Размер можно изменить
                    />
                ) : (
                    <p>Loading...</p>
                )}
            </div>
            <br />
            <form className="form" onSubmit={handleSubmit}>
                <div className="form-group">
                    <label>Имя:</label>
                    <input
                        type="text"
                        id="name"
                        placeholder="Ваше новое имя"
                        value={name}
                        onChange={(e) => setName(e.target.value)}
                    />
                </div>
                <div className="form-group">
                    <label>Фотография:</label>
                    {previewUrl && <img src={previewUrl} alt="Profile Preview" />}
                    <input
                        type="file"
                        accept="image/*"
                        onChange={handleImageChange}
                    />
                </div>
                <button type="submit" className="update-button">
                    Обновить данные
                </button>
            </form>
            <br/>
            <div className="err-message">
                {errorMessage !== "" && (
                    <b>{errorMessage}</b>
                )}
            </div>
        </div>
    );

}
export default ProfilePage;