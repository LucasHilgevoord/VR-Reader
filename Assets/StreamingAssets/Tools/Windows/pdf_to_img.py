# Pip installs:
## pip install pdf2image
## pip install pyinstaller

import sys, os
from pdf2image import convert_from_path

def convert(pdf_path, output_dir):
    current_dir = os.path.dirname(os.path.abspath(__file__))
    poppler_bin = os.path.join(current_dir, "poppler-24.08.0", "Library", "bin")

    pages = convert_from_path(
        pdf_path,
        dpi=200,
        poppler_path=poppler_bin
    )

    os.makedirs(output_dir, exist_ok=True)
    for i, page in enumerate(pages):
        page.save(os.path.join(output_dir, f"page-{i+1:03d}.png"), "PNG")

if __name__ == "__main__":
    if len(sys.argv) < 3:
        print("Usage: pdf_to_img.exe <input.pdf> <output_folder>")
        sys.exit(1)

    convert(sys.argv[1], sys.argv[2])
    print("PDF conversion complete.")
